var path = require('path');
const targetFolder = './sitecore/shell/client/Applications/MarketingAutomation/'

module.exports = {
    entry: './ClientApplication/anonymize-contact.plugin.ts',
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
        filename: 'anonymize-contact.plugin.js',
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