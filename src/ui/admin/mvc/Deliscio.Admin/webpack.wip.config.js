const path = require('path');
const fs = require('fs');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { PurgeCSSPlugin } = require("purgecss-webpack-plugin");

const glob = require('glob-all');

const entryPoints = {};
const reactTsxFolder = './wwwroot/tsx';
//const reactScssFolder = './wwwroot/scss';
const webpack = require('webpack');

fs.readdirSync(reactTsxFolder).forEach((entryFolder) => {
    const tsxEntry = `./${path.join(reactTsxFolder, entryFolder, 'index.ts')}`;
    if (fs.existsSync(tsxEntry)) {
        entryPoints[entryFolder] = tsxEntry;
    }
});

//fs.readdirSync(reactScssFolder).forEach((entryFolder) => {
//    const scssEntry = `./${path.join(reactScssFolder, entryFolder, 'styles.scss')}`;
//    if (fs.existsSync(scssEntry)) {
//        entryPoints[`${entryFolder}.scss`] = scssEntry;
//    }
//});

module.exports = (env, argv) => {
    return {
        entry: entryPoints,
        output: {
            filename: '[name].bundle.js',
            path: path.resolve(__dirname, './wwwroot/modules'),
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
                    test: /\.ts?$/,
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
                        /*MiniCssExtractPlugin.loader,*/
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
            //new MiniCssExtractPlugin({
            //    filename: 'css/[name].css'
            //}),
            //new PurgeCSSPlugin({
            //    paths: glob.sync([
            //        path.join(__dirname, './Views/**/*.cshtml'),
            //        path.join(__dirname, './Library/**/*.tsx'),
            //    ]),
            //    minimize: true,
            //    output: path.join(__dirname, './wwwroot/css/purified.css'),
            //    purifyOptions: {
            //        whitelist: []
            //    }
            //}),
        ],
    };
};
