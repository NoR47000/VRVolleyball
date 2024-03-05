Description : VRVolleyball is, as it name's says a VR Volleyball game that allows the player to simulate the jumping and play with (rudementary) bots. The player uses physics to spike and bump the ball. It is based on the Beach VB game of 2 vs 2.

# VRVolleyball
add Steam VR for it to work
go to unity asset store, download steam VR and open with unity then import all

change pipeline : https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@7.1/manual/InstallURPIntoAProject.html

go to Window>package manager
select Unity Registry
Install Universal RP
Window> rendering> pipeline converter

add shadder for transparency and change alpha for transparency

Go to File >Build settings > player settings > XR-plugin Managment and add Open VR Loader

Go to Window > SteamVR Input, click on save and generate 
Body needs to not interact with itself otherwise the hands arm and body will collide
and create an unstoppable vibration, 
if it does, go to Edit> Project Settings> Physics
find the interaction matrix and unselect the BodyxBody checkbox 
Also unselect Body x Dynamic Object

To start VR don't forget to save and generate the actions in the Window>SteamVR Input

Not necessary :
//Add mesh collider to leftHand and rightHand Objects to player gameObject in the scene (player > steamVRObjects). 
//Unselect the Sphere Collider in both hands.


Then start project
