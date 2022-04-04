# Diplo Media Download for Umbraco

## Overview

This is an **Umbraco 8** package that allows you to download files from the **Umbraco Media Library** as a zip archive. You can download both files and
folders. When downloading a folder it can include any nested folders and preserves the correct paths (so in theory you could download your entire media library in one zip archive file!).

It adds a new menu item - called **Download** - to the **Actions** menu when in the **Media** section. This triggers a dialogue that prompts
you to confirm the download. If the media item selected is a Folder then you can choose whether to include any nested folders within the folder.

For more information read https://www.diplo.co.uk/blog/web-development/diplo-media-download-for-umbraco/

## Screenshots

#### Download Action in Menu

![Download_Dialog2](https://user-images.githubusercontent.com/6676168/161609461-cc085543-4330-467c-ae1c-1c907269d56f.PNG)

#### Download Dialog Prompt

![Download_Dialog3](https://user-images.githubusercontent.com/6676168/161609394-180e3c23-106f-48df-b79d-b6d3658591f0.PNG)

## Download & Installation

Nuget: https://www.nuget.org/packages/Diplo.MediaDownloader/ 

Our Umbraco: https://our.umbraco.com/packages/backoffice-extensions/diplo-media-download/ 

## Structure

`Diplo.MediaDownload` is a .NET Framework class library that contains all the C# code.

`TestUmbracoSite` is a, erm, test Umbraco 8 site (using SQLce and Starter Kit) that can be used to test the package.

The client side code is in the `~/App_Plugins/DiploMedia/` folder of this site.

## Dependencies

Utilises [SharpZipLib](https://github.com/icsharpcode/SharpZipLib) (which ships as a dependency of Umbraco 8) to create the Zip files.

## Test Site Logins

These are demo logins for the example site:

### Admin User

This is a user with access to the Media section:

Email: `admin@example.com`

Password: `7TcNDM&jM[`

### User Without Access

This user doesn't have access to the Media section so shouldn't be able to call the download API controller directly:

Email: `nomedia@example.com`

Password: `sAKlTy[{g=`

Controller: `/Umbraco/backoffice/DiploMedia/Media/Download/12345`

### Building Package(s)

The folder `Buildpackage` contains a batch file called `Build.bat` that can be executed to create both the NuGet and Umbraco package versions.

**Note:** You need it to contain the correct path to `MSBuild.exe` on your system.

If the build goes OK then you'll find the packages in the `BuildPackage\Packages` folder.

