# Design Implementation: 

## Player: 
-------------------------------------------------------------------------------------
We wanted a character for a sci-fi themed platformer and animations for various actions. We made a custom character controller with a move function that would take in input for movement in the x axis as well as if the player should jump or crouch. Unity’s Rigidbody2D component let us control the physics of our object and apply forces for movement or jumping. The player can take three hits before he dies, similar to games like Zelda with the three heart system.

 - **Checking for ground**: One of the most difficult parts of making the controller was in figuring out how to check for the ground and differentiating between walls and floor. We tagged the tiles we would use as tiles and used rays shooting straight downwards from the player model to check if the player was on the ground. The rays must be shot in a box or circle in case the player is on a ledge, where a part of the player is still on the ledge but is no longer counted as standing on the ground. Originally, the player was not able to move while in the air, but the top to bottom structure of the levels made it difficult to drop down a level. 
    
 - **Jump**: The jump must apply enough force to clear the levels so we had to tune it a bit. When jumping, the character follows parabolic motion as a force is applied once at the beginning and gravity is constant throughout the jump. I tried working with a jump that would apply more force when falling, similar to how platformers like Mario use jump mechanics, but it did not fit the level design that works top to down instead of left to right.
    
 - **Crouch**: added a crouch mechanic that would also function to let the player fall quickly to the ground when in midair. Crouching slows the speed the player is moving to half of the original speed. Xeon’s sprite sheet contained some models that could function as a crouch and so it was added in.
    
 - **Roll**: Initially we were going for a dash mechanic that would add force in the direction the player was facing, but it proved to be buggy and the levels did not need a dash mechanic. We changed out the dash for a roll that would provide invincibility during the roll to dodge some enemy hits
    
 - **Shoot**: The player has a laser gun as part of the sci-fi theme. When the z button is pressed, the shooting animation is played and a projectile is spawned at the end      of the animation. The projectile is controlled by physics and moves forward in a direction until colliding with an object. If the object is an enemy, it will deal damage. 
    
 - **Camera**: We needed a camera that would follow the player as he moved so we centered it on the position of the character until death.
    
 - **Events**: The player must know when to stop the jump animation so we made an event that would call a method to stop the jumping animation when the player is landing.

## Enemies: 
-------------------------------------------------------------------------------------
The enemies use the same controller as the player but do not rely on player input to move. Instead, they use random numbers to choose how they will move in a patrol pattern in the area where they spawned. 
    
 - **Patrol**: Enemies will choose a direction to move in as well as a time duration in which to move in that direction. Then, they will pause before deciding again. We used an infinite loop to accomplish this, which exits when the enemy dies and the object is destroyed.

 - **Dealing damage**: We did not have time to add attacks for enemies like shooting lasers, moving towards the player when close, etc. Currently, the enemies deal damage when the player collides with them.

 - **Floating enemies**: Some enemies, like planes, laser arms, floating eyes, etc. will float in the air instead of being on the ground. They are not affected by gravity but behave in the same way.
    
 - **Events**: When an enemy dies, we check for remaining enemies and if there are none, the victory screen is played. Originally we tried having an infinite loop continually checking for enemies, but thought it would be better and save resources if we checked for enemies only when one died.
    
 - **Menus**: The menus should provide buttons to start the game (play), options to change settings such as volume, credits for game art and sounds, and an exit button. We connected events to change the scene, such as when the play button is pressed, the scene is changed to the game scene. When the player dies, a scene is added that allows the character to retry or to return to the main menu. A victory screen occurs when all enemies are killed.

## Levels:
-------------------------------------------------------------------------------------
