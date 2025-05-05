Vector Field Lite V2
This is a tool for creating 3D vector fields in Unity. 


### How to use:

• Launch the window from Window > Vector Field Lite V2.
• Create a 3D Texture from scratch or Import (in Pro Version) a 3D Texture.
• Create arrows from the “+” button or duplicate arrows from Edit > Duplicate Arrow or using the Ctrl+Shift+D shortcut.
• You can also create arrows based on selected game objects in the scene by using the “Selected to Arrows” button.
• Export selected arrows into a .json file which will hold the values of these rows so you can import them as many times as needed in any session.
• You can create only up to 15 arrows, Pro version doesn’t have this limit.


### There are five types of arrows:

• Absolute: affects all pixels by “Force” amount equally.
• Relative: affects pixels within inner radius by full amount of “Force” then fades out in the outer radius.
• Inverse Relative: affects pixels within outer radius by full amount of “Force” then fades out in the inner radius.
• Multiply: multiplies the value of pixels within inner radius by full amount of “Force” then fades out in the outer radius.
• Suck: sets pixels in outer radius to direct towards the arrow (producing a suction effect). If you set the “Force” to a negative value then the direction of the pixels will be in the opposite direction (producing a propelling effect).


### Arrow operations:

• To delete arrows make sure to use the “-” button next to the “+” button rather than deleting the game object.
• You can select the root of an arrow either by clicking at the center piece of an arrow or by clicking on the “☰” button.
• You can hold ctrl to select multiple arrows, you can also hold shift to select a range of arrows.
• The order of arrows is relevant, for example if a multiply arrow is before a relative arrow then there will be no value to multiply. You can drag arrows from the “☰” button to reorder them.
• When multiple arrows are selected changing any property would change all of them. For example if you select all arrows then set the mode of one of them to “Relative” then all arrows will become relative.
• You can select the tip or the base of any arrow and move it to change the direction.


### Texture operations:
• When you open the window, a render texture will be created (if it is not created already) called “RealtimeTexture” in the “Asset” folder.
• You can use this texture (after checking the “Realtime” checkbox) to see all changes to arrows in realtime without having to save a texture.
• There are two types of output textures, Signed which has values in negative or positive, Unsigned which has values between 0 and 1, VFX Graph "Vector Field Force" node supports both types, while the Particle System Force Field component only supports Signed textures.
• For each type of output texture there are different formats:
    Signed RGBAHalf which should be used most of the cases.                             (16 bit per channel, 4 channels)
    Signed RGBAFloat which has more precision but produce a much larger size texture.   (32 bit per channel, 4 channels)
  Unsigned textures occupies less size than Signed ones:
    Unsigned RGB565 is the smallest but is a bit imprecise.                             (5 bit x, 6 bit y, 5 bit z)
    Unsigned RGB24 is a good precision to file size balance.                            (8 bit per channel, 3 channels)
    Unsigned RGB48 gives more precision but takes more size.                            (16 bit per channel, 3 channels)
• When you are happy with the result click “Save Texture” then name the texture and save where you need.


### Customization:

• You can customize arrow tip, mid and base if you wish, just remove the label of the original ones and add it to your custom prefab with the appropriate one.


### Tutorial:

• There is a tutorial video on YouTube that goes though an example:
https://www.youtube.com/watch?v=2c34PTmDhxA

• And for V1:
https://www.youtube.com/watch?v=Mh-zh_Hj0V4


### Pro version:

• The Lite version of the asset have limits on the number of arrows that you can create and doesn’t have the Import feature which gives the ability to import pre made 3D Textures and modify them.
• You can checkout the Pro version here:
http://bit.ly/pro-from-lite