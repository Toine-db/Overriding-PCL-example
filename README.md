#Overriding PCL example

When developing cross-platform you more than often need to write platform specific code or you can’t find portable libraries but only platform specific ones. Patterns like dependency injection could be great tool to use platform specific code or libraries when developing cross-platform. But this can quickly become really messy and shift business logic to the UI projects.

Fortunately there is another not very known solution to handle this, that some people call the ‘Advanced PCL’ or ‘Bait and Switch’ pattern. This is a good solution without needing to hack, to implementing 3rd party components or do any kind of scripting. It’s quite hard to find an example or article about this approach and the few that can be found are, in my opinion, clouded and over complicated by other fancy stuff or described way too complex in relation to how easy this basic functionality inside Visual Studio really is.

Therefore my explanation and example of this approach containing the following subjects:

1. The Principal
2. The Mechanisme
3. The Rules
4. How to: create this in Visual Studio
5. How to: unit test
6. How to: make it more advanced (not included in example)


***


##The Principal
In short; __a platform specific assembly is preferred over portable assemblies.__

Personally it feels more like the way overriding methods work in c#; a platform specific DLL file overrides the portable DLL file. Therefore I call this approach ‘Overriding Libraries’ because this describes the outcome of this approach a little bit more.
##The Mechanism
Within the platform specific library you implement the code that eventually does to work. Within the portable specific library you copy all publics from the platform specific library, just to make it identical.

Reference the platform specific code from your specific platform and reference the portable specific library from your portable projects.

Now comes the strange part! 
You can now reference the portable classes and methods as if you invoke/use the platform specific classes and methods. Yes; Visual studio and resharper still think youre gona use the portable library, but at runtime (like when you are debugging) you are using the platform specific library.
##The Rules
1 keep the libraries identical ‘for the outside’.
In this case identical means the same:
- Project name 		(to get the same library name, like MyLibrary.dll)
- Assembly version 	(to get the same FullyQualifiedAssemblyName)

2 its handy, but not mandatory, to keep the public parts the same. This includes class names, properties, methods names and used parameters. When you don’t you can get the following issues;
- When a public part is only available in the portable version the App will probably normally compile and run, but when that certain parts is invoked you will get something of “Missing method exception”
- When a public part is only available in the platform specific version the App won’t even compile when its referenced inside the code.

## How to: create this in Visual Studio
Visual Studio doesn’t like when you create projects with the same name in a single solution, but there are multiple ways to accomplish this task. The way I do it is simply by creating new dummy solutions with a single project, close the solution again and copy past the project to the solution where you really want to use it. Then add make some folder structure by adding some ‘Solution Folders’ and add the newly created project(s) as ‘Existing Solution..’.. 

The folder structure could look something like;
* MySolution
	* MyApp.Droid (android)
	* MyApp.Core (portable)
	* MyOverridingLibraries (a solution folder)
		* Droid (a solution folder)
			* MyExtraLibrary (the real implementation)
		* Portable (a solution folder)
			* MyExtraLibrary (the dummy one)

##How to: unit Test
You can just normally use Unit tests! Just make sure you reference the platform specific library because you can’t add references with the same name, and without compiling the whole App the overriding mechanism won’t kick in to.

The one thing I definitely wanted to test with this approach was the fact that the overriding libraries were identical. Because .Net reflection can’t work well with libraries with the same name loaded at the same time. To get the qualified assembly name and all others namings I used ‘[Cecil](https://github.com/jbevain/cecil/)’, Cecil can read libraries without loading them or the need to load all internal references to.

##How to: make it more advanced (not included in example)
Visual Studio and Resharper think the portable version is the one the App will work with. This means that differences between the portable and platform versions aren’t detected at compilation, but surely can fail the Unit tests or let the App crash at runtime.

To prevent this you can use linked files for the public classes instead of duplicates for every library. Then again you need to write code in this linked files ‘as if’ you use other internal classes, the can be implemented as dummies again. This way all changes will checked by Visual Studio and Resharper at compile time. 


***


######Other sources

- (article) http://log.paulbetts.org/the-bait-and-switch-pcl-trick/
- (article) http://blogs.msdn.com/b/dsplaisted/archive/2012/08/27/how-to-make-portable-class-libraries-work-for-you.aspx
- (example) https://github.com/xamarin/customer-success/tree/master/samples/Xamarin.iOS/AdvancedPCL 

