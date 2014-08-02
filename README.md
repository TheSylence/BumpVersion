# BumpVersion
> Lets you bump versions in your software

Need a one line command to increase the version number of your shiny software?
Then maybe BumpVersion is right for you. After the initial setup of a project file
describing what should be done when bumping the version, all you ever need to do
is executing a single command to bump the version in your whole application.

# Getting started

Create a file called *bumpversion.xml* in your project folder and fill it with the following XML:

```xml
<?xml version="1.0"?>
<bumpversion>
	<Task type="WriteToFile">
		<key name="files" value="version.txt" />
	</Task>
</bumpversion>
```

This would write the version number to a file called *version.txt* everytime you run the application.

Now if you want to bump the version of your project to 1.2 you would call bumpversion.exe via the command line with
`bumpversion.exe 1.2`

# Tasks

A project file may contain as many tasks as you want to.
For every task you need to add a `Task`-element to your project file.
Also you have to specify what type the task is by providing a `type`-attribute to the element.
If the tasks requires further configuration you can add these configurations by putting `key`-elements inside

## WriteToFile

Writes the version number to a file (will overwrite any content the file already has).
To use it in your project add a `<Task type="WriteToFile">` to your project file.

You need to specify all files the version should be written to by adding a `<key name="files" value="file1;file2;file3;...;fileN" />` inside the element.
You don't have to specify more than one file but if you do, you need to separate each file with a semicolon.

===========
More tasks are to follow!
