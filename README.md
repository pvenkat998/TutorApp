# TutorApp



#Notes for me
NOTES FOR STYYLES XML ERROR
I got this error when I changed the name (and namespace) of my
Xamarin.Android project outside of Visual Studio. I just manually 
deleted the (Droid)\Resources\Resources.Designer.cs file, reopened 
it in VS 2017 and rebuilt it. It regenerates that file if your resources
change (or if it can't find it), and that fixed it for me. Annoying tho, took me ages to figure it out!
