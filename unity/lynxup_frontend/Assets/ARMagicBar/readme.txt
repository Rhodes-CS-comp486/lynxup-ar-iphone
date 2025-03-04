Hi fellow developer! :) 

First of all thanks a bunch for your purchase, I really appreciate it!
If you have any problems or suggestions, feel free to write me at contact@alive-studios.io

Please find a more detailed description and video tutorials here:
https://zealous-system-734.notion.site/AR-Magic-bar-documentation-5341377965584f528017ef8893fee5d4?pvs=74

How to use this asset: 

## The basic setup ##
Make sure that you have already setup your AR Scene (AR Foundation or Lightship needs to be installed) and  meshing or plane 
detection (With an XR Origin,  AR Plane Manager (only for plane detection) and  AR Raycast Manager (only for plane detection)) 
is setup. Note: The setup in 2021 LTS is a bit different, please refer to the AR Foundation documentation: 

Important: When you want to use AR plane detection make sure to add a AR Raycast manager to your XR origin. When using meshing 
or Non-AR 3D, make sure that your AR mesh or 3D objects have a collider attached. You can select plane detection or meshing in 
the AR Magic Bar asset later on. 

Non AR / 3D Scenes are supported too, however only in combination with AR Foundation installed as the Magic Bar uses namespaces of AR Foundation. 
You could use the Magic Bar in a normal full 3D project or switch between AR and 3D scenes , however AR Foundation needs to be installed.

## Add assets to the AR Magic Bar##
On the top bar, click on "Window" and open the AR Magic Bar / Prefab and Image Editor. Attach the window to your view. 

## Prepare your assets##
It is very important to prepare your assets. First and foremost make sure that all assets have some kind of collider attached. 
AR Magic Bar will automatically attach a collider to your object if there is none, however I highly recommend to add 
one by yourself to ensure the collider matches the shape and size of the actual object. 

Also make sure to set a suitable scale and rotation. Of course all of these objects can be manipulated  later on.

The AR Magic bar works with both mesh and skinned renderer, so it also works with any kind of creature as well as
solid objects and particle objects.

Additional to the prepared prefab, you will need a UI Icon.  This should by in squared format e.g. 512 x 512 pixel, but 
any size is ok. You can e.g. simply create a screenshot of your object , drag it into unity and set it to a 2D Sprite (Single). 
Make sure if applicable (e.g. in Unity 6) to set the sprite mode to “Single”.


## Add your assets to the AR MAgic bar##
Click "Add Pair". Now you can drag in your prefab (left side) and Icon (right side). Currently the maximum amount of 
prefabs you can use is 30 at once. Make sure each Pair has a unique prefab and Icon. 

Finally click on "Create Placeable Objects".

## Drag in the AR Magic Bar Asset## 
Get the AR Magic Bar prefab from the AR Magic Bar folder and drag it into the scene. 
Make sure to select the mesh option if you want to use Mesh placement (Works with normal 3D) or 
AR Plane placement or even disable the placement when you want to use it for something else. 
You need to set this before starting the game.

DONE! Time to click "play". Now you can place and manipulate your assets by clicking on the Icon and 
place it on the plane or mesh and then clicking on the item again.

# Potential error fix#
Make sure that the "PlaceableObjectDatabase", scriptable object in Project/Assets/ARMagicBar/PlaceableObjectDatabase does not have any "missing", objects. 
If it does, make sure to delete them by clicking on the "-". In some occasions e.g. when removing the full asset from your project and re-adding it again or when 
restarting the editor while placement-objects have not been created properly, this error might appear.

Please find a more detailed description and video tutorials here:
https://zealous-system-734.notion.site/AR-Magic-bar-documentation-5341377965584f528017ef8893fee5d4?pvs=74

Legal notice: 
This asset was created by Tobias Reidl from Alive Studios. 
You are not allowed to redistribute or share this asset or parts of it in any way. You are allowed to use this asset within any of your Unity projects 
(commercial or non commercial) for the intended purposes. 
