import { defineConfig } from 'vite';
import { resolve } from 'path';

export default defineConfig({
   build: {
      outDir: 'wwwroot/client',
        emptyOutDir: true,
      sourcemap:true,
      rollupOptions: {
         input: {
            app: resolve(__dirname, 'wwwroot/tsx/app.ts'),
            home: resolve(__dirname, 'wwwroot/tsx/pages/Home/index.ts'),
            links: resolve(__dirname, 'wwwroot/tsx/pages/Links/index.ts'),
            users: resolve(__dirname, 'wwwroot/tsx/pages/Users/index.ts'),
         },
         output: {
            entryFileNames: '[name].mjs',
            chunkFileNames: '[name].mjs',
            assetFileNames: '[name].[ext]',
         },
      },
   },
   resolve: {
      alias: {
         '@images': resolve(__dirname, 'wwwroot/imgs'),
         '@apis': resolve(__dirname, 'wwwroot/tsx/apis'),
         '@components': resolve(__dirname, 'wwwroot/tsx/components'),
         '@pages': resolve(__dirname, 'wwwroot/tsx/pages'),
         '@utils': resolve(__dirname, 'wwwroot/tsx/utils'),
         //'@styles': resolve(__dirname, 'wwwroot/src/tsx/styles'),
         '@': resolve(__dirname, 'wwwroot'),
      },
   },
});
