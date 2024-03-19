import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
    build: {
        outDir: 'wwwroot/client',
        emptyOutDir: true,
        rollupOptions: {
            input: {
                app: resolve(__dirname, 'wwwroot/src/tsx/app.ts'),
                home: resolve(__dirname, 'wwwroot/src/tsx/pages/home/index.ts'),
                links: resolve(__dirname, 'wwwroot/src/tsx/pages/links/index.ts'),
            },
            output: {
                entryFileNames: '[name].js',
                chunkFileNames: '[name].js',
                assetFileNames: '[name].[ext]',
            },
        },
    },
    resolve: {
        alias: {
            '@components': resolve(__dirname, 'wwwroot/src/tsx/components'),
            '@images': resolve(__dirname, 'wwwroot/imgs'),
            '@pages': resolve(__dirname, 'wwwroot/src/tsx/pages'),
            //'@styles': resolve(__dirname, 'wwwroot/src/tsx/styles'),
            '@utils': resolve(__dirname, 'wwwroot/src/tsx/utils'),
            /*'@': resolve(__dirname, 'wwwroot'),*/
        },
    },
});