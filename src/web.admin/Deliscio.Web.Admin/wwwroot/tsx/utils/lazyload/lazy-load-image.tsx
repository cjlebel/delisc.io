import { StripDataAttributes } from '../dom';

/**
 * Looks up all images that have the 'lazy' class and lazy loads them.
 */
export default function LazyLoadImage() {
    const lazyImages = Array.from(document.querySelectorAll('img.lazy')) as HTMLElement[];

    if ('IntersectionObserver' in window) {
        let lazyImageObserver = new IntersectionObserver(function (entries, observer) {
            entries.forEach(function (entry) {

                if (entry.isIntersecting) {

                    if (entry.target instanceof HTMLImageElement) {

                        lazyLoadImage(entry.target);

                        observer.unobserve(entry.target);
                    }

                }

            });

        });

        lazyImages.forEach(function (lazyImage) {
            lazyImageObserver.observe(lazyImage);
        });
    } else {

        // In case it's an older browser that doesn't support 'IntersectionObserver'
        lazyImages.forEach(function (lazyImage) {
            const img = lazyImage as HTMLImageElement;
            lazyLoadImage(img);
        });

    }

    /**
     * Lazy loads an image by setting the 'data-src' attribute to the 'src' attribute
     * @param {HTMLImageElement} image
     */
    function lazyLoadImage(image: HTMLImageElement) {
        if (image) {

            try {

                const dataSetSrc = image.dataset.src ?? '';

                if (dataSetSrc === '')
                    return;

                image.src = dataSetSrc;
                image.classList.remove('lazy');

            } catch (e) {

                console.error('Error loading image', e);

            }

            StripDataAttributes(image);
        }
    }
}
