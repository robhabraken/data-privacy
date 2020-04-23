# Angular Client application for custom Marketing Automation activity

Webpack will output the built JavaScript plugin in this folder. The compiled code is not included in the project or repository, but generated at build time with the following command in the root folder of this Feature:

`npm run build`

You can also use the .\build.ps1 script in the project root to install node and run the build in one go.
To make sure the output is copied over to your Sitecore target installation, the JavaScript content of this folder is added to the solution using a wild card in the solution file, but ignored in Git to not commit compiled code.