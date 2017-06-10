Quick architecture discussion:

This solution uses 'Shared Projects' in the way most solutions would use Libraries.
This is because it has been written with Monogame's ability to port to multiple 
platforms in mind. As such, we don't necessarily have access to dlls - and 
everything will have to be recompiled. It was that or shared source files - which
seemed uglier and more maintenance heavy to me.

The projects are as follows:

  - GLGameApp
    The 'Game' application for desktop users

  - AtlasWarriorsGame
    Shared project that contains the actual game objects and logic.  The models
	and controllers to GLGameApp's view.

  - AtlasWarriorsGame.Tests
    NUnit test project for AtlasWarriorsGame.

  - UI Common
    Stuff that multiple interfaces need, but that I still want to keep
	seperated.