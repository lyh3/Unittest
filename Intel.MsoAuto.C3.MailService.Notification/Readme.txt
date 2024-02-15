A standard library for MSO Automation Applications that provides a mail notification
To learn more read the project web page.

Target Framework: .NET 6 

To Create and Push Nuget Package Version
----------------------------------------
1) Update the Assembly Version (must do to push).
	- Right click on the project and select Properties.
	- Increment the Assembly version. 
		* The packaging version and File Version should be set to $(AssemblyVersion) to keep these in sync. 
2) Building in Visual Studio will automatically create the .nupkg file in bin/Debug/.
	-If not check the project Properties under Package -> General. Check box "Generate NuGet package on build"
3) Copy the file to the project folder (where the nugetpush.bat is)
4) Edit the nugetpush.bat file to increment the nupkg file name being pushed - (nuget push Intel.MsoAuto.Shared.1.0.0.1.nupkg)
5) Run the nugetpush.bat and check the Nuget Package Manager to verify that the push was successful.

This package expects the following packages:
