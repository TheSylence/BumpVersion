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
If the tasks requires further configuration you can add these configurations by putting `key`-elements inside the task.
The `key`-element needs a `name`-attribute and a `value`-attribute.

## WriteToFile

Writes the version number to a file (will overwrite any content the file already has).

Key | Description | Required?
--- | ------------ | ----------
files | List of files the version number should be written to. | Yes


## WixProductID

Generates a new ProductID in a WiX project file. This is useful if you want to create an installer that
can update previous installations.

Key | Description | Required?
--- | ------------ | ----------
wixFile | Name of the WiX project file that should be updated. | Yes


## ReadVSProject

Reads files from a Visual Studio Project file and stores them in a variable. This is useful if you want to
apply some operations to a whole bunch of files that are part of a Visual Studio project.

Key | Description | Required?
--- | ------------ | ----------
projectFile | Name of the project file that should be parsed. | Yes
output | Name of the variable the parsed files will be stored in. | Yes
elements | List of elements that should be searched for when parsing the project. | No (default is *Compile*, *Page* and *None*)

## CreateVariable

Creates a variable with a given name and value that can be used in other tasks.

Key | Description | Required?
--- | ------------ | ----------
name | Name of the variable to create | Yes
value | Value of the variable | Yes

===========
More tasks are to follow!
