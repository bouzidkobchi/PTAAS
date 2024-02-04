# PTAAS

<!--![1701723963858](https://github.com/bouzidkobchi/PTAAS/assets/106019236/afb3e631-e337-4a4e-a382-0fb7aea3e6b8)-->

## Requirements :
1 - git

2 - dotnet version 8 (net7 can run the project but maybe be it cause some issues with the dependencies)

3 - Sql Server

4 - all dependencies will be installed when building the project for the first time

## Running steps :

1 - clone the repository :

cmd > `git clone https://github.com/bouzidkobchi/PTAAS.git`

<hr />

2 - go inside the ptaas folder :

cmd > `cd ptaas`

<hr />

3 - run the project :

cmd > `dotnet run --project webapi`

or :

cmd > `cd webapi`
cmd > `dotnet run`

<hr />

and for custom port use this :

cmd > `dotnet run --project webapi --urls=http://localhost:{your_custom_port}`

or :

cmd > `cd webapi`
cmd > `dotnet run --urls=http://localhost:{your_custom_port}`

<hr />

3.5 - go to this page : 

`localhost:{your_post_or_from_cmd}/swagger/index.html`

my advice : run it from visual studio (ctrl+f5) and wait until it redirect you to the swagger ui page .

4 - modify the database connection string :

if all previous steps run correctly and you get the swagger ui page, modify the connection string in `appsettings.json` , you can take it from visual studio > server explorer window > select you db > properties > connection string 
also you can use your own , otherwise ask me .

## possible Exceptions:
0 - don't care about XML warnings , they are just used for code documentation (i will do it later) , if you wanna hide them :
run your app , clean your windows (using `cls`) , and run it again.

1 - running more than one instance :
```
C:\Program Files\dotnet\sdk\8.0.101\Microsoft.Common.CurrentVersion.targets(5198,5): error MSB3027: Could not copy "C:\
Users\bouzid\Desktop\New folder (3)\PTAAS\webapi\obj\Debug\net8.0\apphost.exe" to "bin\Debug\net8.0\WebApi.exe". Exceed
ed retry count of 10. Failed. The file is locked by: "WebApi (1900)" [C:\Users\bouzid\Desktop\New folder (3)\PTAAS\weba
pi\WebApi.csproj]
C:\Program Files\dotnet\sdk\8.0.101\Microsoft.Common.CurrentVersion.targets(5198,5): error MSB3021: Unable to copy file
 "C:\Users\bouzid\Desktop\New folder (3)\PTAAS\webapi\obj\Debug\net8.0\apphost.exe" to "bin\Debug\net8.0\WebApi.exe". T
he process cannot access the file 'path_to_your_local_repo\PTAAS\webapi\bin\Debug\net8.0\WebApi.exe' bec
ause it is being used by another process. [path_to_your_local_repo\PTAAS\webapi\WebApi.csproj]

The build failed. Fix the build errors and run again.
```

to fix it , just check you cmd windows you are mostly running more than one instance .

2 - if you have any exceptions from the dependencies try to clean the project :
cmd > `dotnet clean --project webapi`
or :
cmd > `cd webapi`
cmd > `dotnet clean`

# otherwise just ask me
