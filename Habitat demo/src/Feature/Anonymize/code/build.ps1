# re-install node because HabitatHome build breaks node_modules now and then (?) and to make the process repeatable
Remove-Item -path node_modules -recurse -force
npm install

# run webpack build
npm run build