﻿using System;
using System.Collections.Generic;
using System.Linq;
using static AtlasWarriorsGame.Dungeon;
using static AtlasWarriorsGame.GlobalRandom;

namespace AtlasWarriorsGame.DungeonGenerators
{
    /// <summary>
    /// Generator for level with multiple rooms, tightly packed
    /// </summary>
    static partial class RoomsGenerator
    {
        const int MAX_ROOM_SIZE = 10;
        const int MAX_FAILURES = 10000;

        /// <summary>
        /// Generate a 'normal' dungeon
        /// </summary>
        /// <param name="Dungeon"></param>
        /// <returns>True on success</returns>
        public static bool Generate(Dungeon Dungeon)
        {
            // Set start to middle of room
            Dungeon.StartLocation = new XY(Dungeon.Width / 2, Dungeon.Height / 2);

            // Until filled, place rooms!

            var possibleFeatures = GenerateFeatureLibrary();
            Feature nextFeature;
            var possibleDoors = new List<XY>();
            // Create starting door
            possibleDoors.Add(new XY(R.Next(Dungeon.Width - 4) + 2,
                R.Next(Dungeon.Height - 4) + 2));
            var successfulDoors = new List<XY>();

            int failures = 0;

            // Unsure if this should be true, or lambda - this seemed neat enough
            while (
                (failures < MAX_FAILURES) &&
                (possibleDoors.Count > 0) &&
                Dungeon.AnyEmpty() )
            {
                // Get random feature, in random rotation
                nextFeature = possibleFeatures[R.Next(possibleFeatures.Count)]
                    .Rotate((Feature.Rotation)R.Next(4));

                // Choose a door to build on
                var baseDoor = possibleDoors[R.Next(possibleDoors.Count)];

                // List to put potentials in - doors on the opposite extremity in the feature
                // to the free side of the base door
                var possibleFeatureDoors = new List<XY>();

                // Randomise order of featureDoors to make any possible
                possibleFeatureDoors = nextFeature.PossibleDoors.OrderBy((i) => 
                    R.NextDouble()).ToList();

                // Whether the feature was successfully placed
                bool placed = false;

                foreach (XY featureDoor in possibleFeatureDoors)
                {
                    // Amount to add to feature to translate to global - this is baseDoor - door -
                    // so, for instance - base door X of 20, and door x of 5 would mean we need
                    // to add 15 to the feature x to translate it to the global x
                    var translate = baseDoor - featureDoor;

                    // If feature fits...
                    if (FeatureFits(translate, Dungeon, nextFeature))
                    {
                        // Add the feature
                        AddFeature(translate, Dungeon, nextFeature);

                        // And add its doors to the possible door list
                        possibleDoors.AddRange(nextFeature.PossibleDoors.Select(
                            i=>(i + translate)));


                        {
                            // Clear any doors that are no longer usable
                            possibleDoors = possibleDoors.Where(door =>
                            {
                                return
                                    // If in boundaries...
                                    (door.X > 1) &&
                                    (door.X < (Dungeon.Width - 1)) &&
                                    (door.Y > 1) &&
                                    (door.Y < (Dungeon.Height - 1)) &&
                                    // And at least one part there is free
                                    (
                                        Dungeon.GetCell(new XY(door.X - 1, door.Y)) == DungeonCell.EMPTY ||
                                        Dungeon.GetCell(new XY(door.X + 1, door.Y)) == DungeonCell.EMPTY ||
                                        Dungeon.GetCell(new XY(door.X, door.Y - 1)) == DungeonCell.EMPTY ||
                                        Dungeon.GetCell(new XY(door.X, door.Y + 1)) == DungeonCell.EMPTY
                                    );
                            }).ToList();
                        }

                        // And go try a new feature
                        placed = true;
                        break;
                    }
                }

                if (placed)
                {
                    // That door was part of it!
                    successfulDoors.Add(baseDoor);
                }
                else 
                {
                    // We failed. If we fail too often, we're done.
                    failures += 1;
                }
            }

            // Make doors doors
            foreach (var door in successfulDoors)
            {
                Dungeon.SetCell(door, DungeonCell.CLOSED_DOOR);
            }

            return true;
        }

        /// <summary>
        /// Add a feature to the dungeon, and the filled array
        /// Used by the generator
        /// Non empty cells of the feature overwrite
        /// </summary>
        /// <param name="Translate">Translation to add to feature to be global coordinates</param>
        /// <param name="Filling">If space is filled</param>
        /// <param name="Dungeon">Dungeon object to build</param>
        /// <param name="Feature">Feature to add</param>
        public static void AddFeature(XY Translate, Dungeon Dungeon, Feature Feature)
        {
            for (int ix = 0; ix < Feature.Width; ++ix)
            {
                for (int iy = 0; iy < Feature.Height; ++iy)
                {
                    if (Feature.GetCell(new XY(ix, iy)) != DungeonCell.EMPTY)
                    {
                        Dungeon.SetCell(new XY(ix, iy) + Translate, 
                            Feature.GetCell(new XY(ix, iy)));
                    }
                }
            }
        }

        /// <summary>
        /// Check if a feature will fit in the dungeon - meaning that any non-empty (on either) 
        /// cells must be the same.
        /// Used by the generator
        /// </summary>
        /// <param name="Translate">Translation to add to feature to be global coordinates</param>
        /// <param name="Filling">If space is filled</param>
        /// <param name="Dungeon">Dungeon object to build</param>
        /// <param name="Feature">Feature to add</param>>
        /// <returns>True if feature will fit</returns>
        public static bool FeatureFits(XY Translate, Dungeon Dungeon, Feature Feature)
        {
            for (int ix = 0; ix < Feature.Width; ++ix)
            {
                for (int iy = 0; iy < Feature.Height; ++iy)
                {
                    var featureCell = Feature.GetCell(new XY(ix, iy));
                    var dungeonCell = Dungeon.GetCell(new XY(ix, iy) + Translate);

                    // If out of bounds
                    if (((ix + Translate.X) >= Dungeon.Width) ||
                        ((iy + Translate.Y) >= Dungeon.Height) ||
                        ((ix + Translate.X) < 0) ||
                        ((iy + Translate.Y) < 0))
                    {
                        return false;
                    }

                    // If both aren't empty, and if they are - they're not the same
                    if ((featureCell != DungeonCell.EMPTY) && 
                        (dungeonCell != DungeonCell.EMPTY) &&
                        (featureCell != dungeonCell))
                    {
                        return false;
                    }
                }
            };
            return true;
        }

        /// <summary>
        /// Generate the library of features (rooms) that might be added to a dungeon
        /// </summary>
        /// <remarks>This might better load from a file in the future</remarks>
        public static List<Feature> GenerateFeatureLibrary(int MaxRoomSize = MAX_ROOM_SIZE)
        {
            var features = new List<Feature>();

            // Make a bunch of basic rooms
            // Starting with basic rectangles with doors
            // Just narrow ones - because the generator attempts rotations itself.
            // We also do doors in different positions
            for (int iw = 3; iw <= MaxRoomSize; ++iw)
            {
                for (int ih = 3; ih <= Math.Min(iw, MaxRoomSize); ++ih)
                {
                    features.Add(CreateFeature(iw, ih));
                }
            }

            return features;
        }

        /// <summary>
        /// Create basic empty room with given parameters
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static Feature CreateFeature(int Width, int Height)
        {
            var f = new Feature(Width, Height);

            // Fill with floor
            for (int ix = 1; ix < (Width - 1); ++ix)
            {
                for (int iy = 1; iy < (Height - 1); ++iy)
                {
                    f.SetCell(new XY(ix, iy), DungeonCell.FLOOR);
                }
            }

            // Fill walls in
            for (int ix = 0; ix < Width; ++ix)
            {
                f.SetCell(new XY(ix, 0), DungeonCell.WALL); 
                f.SetCell(new XY(ix, Height - 1), DungeonCell.WALL); 
            }
            for (int iy = 0; iy < Height; ++iy)
            {
                f.SetCell(new XY(0, iy), DungeonCell.WALL);
                f.SetCell(new XY(Width - 1, iy), DungeonCell.WALL);
            }

            // Add possible doors
            // If even number of cells, then where ix/iy is odd, or even if it's odd
            for (int ix = 1; ix < (Width - 1); ++ix)
            {
                if ((ix + (Width % 2 + 1)) % 2 == 0)
                {
                    f.AddPossibleDoor(new XY(ix, 0));
                    f.AddPossibleDoor(new XY(ix, Height - 1));
                }
            }
            for (int iy = 1; iy < (Height - 1); ++iy)
            {
                if ((iy + (Height % 2 + 1)) % 2 == 0)
                {
                    f.AddPossibleDoor(new XY(0, iy));
                    f.AddPossibleDoor(new XY(Width - 1, iy));
                }
            }

            return f;

        }
    }
}