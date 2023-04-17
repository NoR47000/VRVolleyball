# VRSmash
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

Then start project
