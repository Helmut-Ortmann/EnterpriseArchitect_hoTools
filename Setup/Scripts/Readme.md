## Version in *msi filename

see: http://kentie.net/article/wixnameversion/index.htm

### Scripts

The following scripts are renaming the *.msi file according to the product version of the product like:

'hoTools 3.1.7.msi'

### Post-buid Event Command Line

In the WIX setup project add:
"$(SolutionDir)\Setup\Scripts\renamemsi.bat" "$(targetPath)"