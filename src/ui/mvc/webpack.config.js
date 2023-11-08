const path = require('path');
const fs = require('fs');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { PurgeCSSPlugin } = require("purgecss-webpack-plugin");

const glob = require('glob-all');
//const PATHS = {
//    src: path.join(__dirname, "src"),
//};

const entryPoints = {};
const reactFolder = './Library/src';
const webpack = require('webpack');

fs.readdirSync(reactFolder).forEach((entryFolder) => {
    const entryPath = `./${path.join(reactFolder, entryFolder, 'index.tsx')}`;
    if (fs.existsSync(entryPath)) {
        entryPoints[entryFolder] = entryPath;
    }
});

module.exports = (env, argv) => {
    return {
        entry: entryPoints,
        output: {
            filename: '[name].bundle.js',
            path: path.resolve(__dirname, './wwwroot/js'),
            clean: true,
        },
        optimization: {
            splitChunks: {
                chunks: 'all',
                cacheGroups: {
                    vendors: {
                        test: /[\\/]node_modules[\\/]/,
                        name: 'vendors',
                        chunks: 'all',
                    },
                    styles: {
                        name: 'styles',
                        test: /\.css$/,
                        chunks: 'all',
                        enforce: true,
                    },
                },
            },
        },
        devtool: 'source-map',
        module: {
            rules: [
                {
                    test: /\.js?$/,
                    use: {
                        loader: 'babel-loader',
                        options: {
                            presets: ['@babel/preset-react', '@babel/preset-env'],
                        },
                    },
                },
                {
                    test: /\.tsx?$/,
                    exclude: /node_modules/,
                    use: {
                        loader: 'babel-loader',
                        options: {
                            presets: [
                                '@babel/preset-typescript',
                                '@babel/preset-react',
                                '@babel/preset-env',
                            ],
                        },
                    },
                },
                {
                    test: /\.scss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        'css-modules-typescript-loader',
                        {
                            loader: 'css-loader',
                            options: {
                                modules: true,
                                sourceMap: true,
                            },
                        },
                        'sass-loader',
                    ],
                },
                {
                    //test: /\.css$/,
                    //use: [
                    //    MiniCssExtractPlugin.loader, {
                    //        loader: 'css-loader',
                    //        options: {
                    //            modules: true,
                    //            sourceMap: true,
                    //        },
                    //    },
                    //],
                },
            ],
        },
        externals: {
            react: 'React',
            'react-dom': 'ReactDOM',
        },
        resolve: {
            extensions: ['.tsx', '.ts', '.jsx', '.js'],
        },
        plugins: [
            new CleanWebpackPlugin(),
            new MiniCssExtractPlugin({
                filename: '../css/app.css'
            }),
            new PurgeCSSPlugin({
                paths: glob.sync([
                    path.join(__dirname, './Views/**/*.cshtml'),
                    path.join(__dirname, './Library/**/*.tsx'),
                ]),
                minimize: true,
                output: path.join(__dirname, './wwwroot/css/purified.css'),
                purifyOptions: {
                    whitelist: []
                }
            }),
        ],
    };
};
