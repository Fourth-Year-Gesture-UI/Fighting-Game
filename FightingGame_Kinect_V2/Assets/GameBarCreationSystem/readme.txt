Thank You for purchasing the Game Bar Creation System.

Includes:
---------

ScrollBarEssentials.cs
ExperienceSystem.cs
HealthSystem.cs
ManaSystem.cs
ExampleGUIAspectsController.cs
GUIController.cs
GlobeBarSystem.cs

---------------------------------------
Updates:
---------------------------------------
[5/10/2012]
Added a GlobeBarSystem which allows for filled circular bars

---------------------------------------
[3/12/2012]
Updated ScrollBarEssentials to contain IncrimentBar as a virtual function
 - Changed ExperienceSystem, HealthSystem and ManaSystem to work with IncrimentBar

Updated the GUIController to prevent public release of the various Systems
 - If your system uses the public members feel free to add it back in
---------------------------------------

Description:
------------

The purpose of the Game Bar Creation System is to allow you to easily create your own game bar systems. Such as health, mana, experience, stamina, loading or any other type of bar that is associated with game data needed for visual incrimentation. An Experience, Mana, and Health system are provided as a template for each of these base systems. However, deriving from the ScrollBarEssentials class or modifying these systems, you can create any type of system needed, as the provided templates do.

The GUIController provides a base template that can be incorporated into your design, attached to your main camera. Similarly, the ExampleGUIAspectsController is used on the main camera within the provided example scene, to demonstrate how your bars can be utilized.

To install within your own application:
---------------------------------------

The best way to install your Game Bar Systems is to attach them to a GUIController script, that is attached to your main camera. The GUIController is the main base for the individual variable implimentation, such as the Dimensions of the Game Bar (x, y, width, height), dimensions of the Game Bar Scrolling Texture, whether it is a verticle bar, the textures it contains, and its rotation (The Game Bars are rotated around the center of the size of that Game Bar System). The GUIController is also responsible for initializing, drawing, and updating the individual bars.

- If your bar is non-vertical
    - Make sure your height within your dimensions input is the height of the bar texture
    - It is always advised to have your height variable a multiple of your width variable
    - The Health Bar provides a good example of how to use this alignment

- If your bar is verticle
    - Make sure your width with your dimensions input is the width of the bar texture
    - It is always advised to have your width variable a multiple of your height variable
    - The Experience Bar provides a good example of how to use this alignment

- If your bar needs particular Scroller Dimensions
    - Remember that the x and y coordinate are based off the top left location of the Main Bar
    - This is useful for if you have a interestingly shaped scroll area for your main bar, you can easily place where you want your bar to scroll from.
    - The Mana Bar provides a good example of how to use this attribute

- To change the value of your bar that is Associated with ScrollBarEssentials, use the overridable function: IncrimentBar(int value).
  Where value will incriment that bar by a certain amount, whether negative or positive. You can see this being used in the provided
  templates.

Please Contact: m11hut@gmail.com if you have any suggestions, bug reports or input.
A Reference To Chase Hutchens would be much appreciated if used in your project(s)
Also, please take the time to rate this Asset and give feedback, if you can. It is very much appreciated. :)