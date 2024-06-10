const webpack = require('webpack');
const path = require('path');

module.exports = {
    entry: './wwwroot/ts/admin/userList.ts',
    module: {
        rules: [{
            test: /\.tsx?$/,
            use: 'ts-loader',
            exclude: /node_modules/,
        },],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    output: {
        filename: "userList.js",
        path: path.resolve(__dirname, 'wwwroot/js'),
    },
    plugins: [
        new webpack.ProvidePlugin({
            $: 'jquery',
            jQuery: 'jQuery',
            'window.jQuery': 'jquery',
            Popper: ['popper.js', 'default']
        }),
    ],
    mode: "development"
}