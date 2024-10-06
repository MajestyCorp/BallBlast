## Arcanoid-shooter prototype
It took me a day and a half to develop this prototype.<br>
The main focus is on working with bullets. They can fly very fast and there can be a lot of them. Regardless of the fps, the distance between the bullets will be the same. As the rate of fire increases, the distance decreases.

![1](https://github.com/user-attachments/assets/de68dcf9-b053-41c1-b459-19c406980237)

To store the bullets, I used a structure that contains the bullet spawn position and the time stamp when the bullet appeared. This data is enough to draw all the bullets through the API `Graphics.DrawMeshInstanced`.<br>
The screen shows the current number of bullets and the frame counter.<br> 
Even with 7-10 thousand bullets, the game plays at 60 fps on low devices.<br>
All code is available for public use.

![2](https://github.com/user-attachments/assets/14f9464a-1f0f-4d86-a4f9-bca71bd247b1)
