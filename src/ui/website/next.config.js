const withBundleAnalyzer = require('@next/bundle-analyzer')({
   enabled: process.env.ANALYZE === 'true',
});

module.exports = withBundleAnalyzer({
   reactStrictMode: true,
   swcMinify: true,
   // pageExtensions: ['js', 'jsx', 'md', 'mdx'],
   // eslint: {
   //    dirs: ['pages', 'components', 'lib', 'layouts', 'scripts'],
   // },
   // i18n: {
   //    locales: ['en-US', 'en-CA'],
   //    defaultLocale: 'en-US',
   //    localeDetection: false,
   // },
   images: {
      domains: ['dummyimage.com'],
      // This isn't advised. Need a "media" server to pass the images through.
      remotePatterns: [
         {
            protocol: 'https',
            hostname: '**',
         },
         {
            protocol: 'http',
            hostname: '**',
         },
      ],
   },
   trailingSlash: false,
   // async headers() {
   //   return [
   //     {
   //       source: "/(.*)",
   //       headers: securityHeaders,
   //     },
   //   ];
   // },
   webpack: (config, { dev, isServer }) => {
      config.module.rules.push({
         test: /\.svg$/,
         use: ['@svgr/webpack'],
      });

      // if (!dev && !isServer) {
      //   // Replace React with Preact only in client production build
      //   Object.assign(config.resolve.alias, {
      //     "react/jsx-runtime.js": "preact/compat/jsx-runtime",
      //     react: "preact/compat",
      //     "react-dom/test-utils": "preact/test-utils",
      //     "react-dom": "preact/compat",
      //   });
      //}

      return config;
   },
});
