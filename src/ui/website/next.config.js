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
      domains: ['dummyimage.com', 'rozenmd-cards.netlify.app'],
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
   // ⚠ Invalid next.config.js options detected:
   // ⚠     The value at .experimental.webVitalsAttribution[0] must be a string but it was a number.
   // ⚠     The value at .experimental.webVitalsAttribution[0] must be one of: "CLS", "FCP", "FID", "INP", "LCP", or "TTFB".
   //    experimental: {
   //       webVitalsAttribution: ['CLS' | 'FCP' | 'FID' | 'INP' | 'LCP' | 'TTFB'],
   //    },
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
