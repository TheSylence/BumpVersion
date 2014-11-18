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

The `value`-attribute either takes a normal value like `fileName` or `123` or a variable that was created by a task **prior** to the current one. To pass a variable use `@variable_name` as the value of the `value`-attribute.

## CreateVariable

Creates a variable with a given name and value that can be used in other tasks.

Key | Description | Required?
--- | ------------ | ----------
name | Name of the variable to create | Yes
value | Value of the variable | Yes

## GitCommit

Commits changes to a git repository.

Key | Description | Required?
--- | ------------ | ----------
message | The message to use while committing. | No (default is "Bump version to {0}"). Omitting the {0} token will result in a warning.

## ReadVSProject

Reads files from a Visual Studio Project file and stores them in a variable. This is useful if you want to
apply some operations to a whole bunch of files that are part of a Visual Studio project.

Key | Description | Required?
--- | ------------ | ----------
projectFile | Name of the project file that should be parsed. | Yes
output | Name of the variable the parsed files will be stored in. | Yes
elements | List of elements that should be searched for when parsing the project. | No (default is *Compile*, *Page* and *None*)

## ReplaceInFile

Replaces all occurrences in the given files of the current version with the one being bumped to.

Key | Description | Required?
--- | ------------ | ----------
files | List of files that should be searched | Yes
search | The regular expression to to search for | No (default is `\b(OLD_VERSION)\b([^\.]){1}` with `OLD_VERSION` being the regex-escaped value of the old version. So for 1.2.3 this would be 1\.2\.3
replace | The value used to replace all found occurrences | No (default is `NEW_VERSION$2`with `NEW_VERSION` being the version you bump to.

## WixProductID

Generates a new ProductID in a WiX project file. This is useful if you want to create an installer that
can update previous installations.

Key | Description | Required?
--- | ------------ | ----------
wixFile | Name of the WiX project file that should be updated. | Yes

## WriteToFile

Writes the version number to a file (will overwrite any content the file already has).

Key | Description | Required?
--- | ------------ | ----------
files | List of files the version number should be written to. | Yes

## ImageWatermark

Prints the new Version number on an image. This can be used to automatically create splash screens
with the correct version number for example.

Key | Description | Required?
--- | ------------ | ----------
source | File name of the image that will be used as a template | Yes
dest | File name to save the watermarked image to. Given in the form X;Y;W;H (W and H can be ommited to use the entire remaining image) | Yes
rect | Rectangle (in pixels) that defines where to print the version to. | No (default is to use the entire image)
color | Color that should be used to print the text. Can be specified like colors in HTML | No (default is white)
font | Font that should be used to print the text. Specify as Font:Size. When Size is omitted 12 pts will be used. | No (default is Arial in 12 pt)

===========
More tasks are to follow!
