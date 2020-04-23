var path = require('path');
const targetFolder = './sitecore/shell/client/Applications/MarketingAutomation/'

module.exports = {
    entry: './ClientApplication/erase-form-submissions.plugin.ts',
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: path.resolve(__dirname, "node_modules")
            }
        ]
    },
    resolve: {
        extensions: [".ts", ".js"]
    },
    output: {
        path: path.resolve(targetFolder, 'plugins'),
        filename: 'erase-form-submissions.plugin.js',
        library: "publishActivities",
        libraryTarget: "umd"
    },
    devtool: 'source-map',
    externals: [
        "@sitecore/ma-core",
        "@angular/core",
        "@ngx-translate/core"
    ]
};