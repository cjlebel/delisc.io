const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const glob = require('glob');

module.exports = {
    entry: {
        'js/vendor.bundle': ['./src/scripts/vendor.ts'],
        'js/common.bundle': ['./src/scripts/common.ts'],
        ...getPagesEntryPoints(),
    },
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, 'wwwroot/dist'),
    },
    resolve: {
        extensions: ['.ts', '.js'],
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader',
                    'sass-loader',
                ],
            },
        ],
    },
    plugins: [
        new MiniCssExtractPlugin({
            filename: 'css/[name].css',
        }),
    ],
};

function getPagesEntryPoints() {
    const pagesEntryPoints = {};

    // Use glob to find all 'index.ts' files within 'Pages' and its subfolders
    const pageFiles = glob.sync('./src/scripts/Pages/**/index.ts', {
        ignore: ['./src/scripts/Pages/**/index.tsx'],
    });

    pageFiles.forEach((file) => {
        // Convert the file path into a valid entry point key
        const entryPointKey = `js/${path.relative('./src/scripts', file).replace('.ts', '.bundle')}`;

        // Add the entry point to the result
        pagesEntryPoints[entryPointKey] = [file];
    });

    return pagesEntryPoints;
}
