//import NoImage from '@images/no-image-found.png';

export default function LazyLoad() {

    console.log('LazyLoad loaded');

    //const noImage = '/imgs/no-image-found.png';
    const images = Array.from(document.querySelectorAll('img.lazy'));

    if ('IntersectionObserver' in window) {

        const observer = new IntersectionObserver((entries, observer) => {

            entries.forEach(entry => {

                console.log('IntersectionObserver');

                if (!entry.isIntersecting)
                    return;

                const image = entry.target;
                const dataSrc = image.getAttribute("data-src") ?? '';

                if (dataSrc) {

                    try {

                        image.setAttribute("src", dataSrc);

                    } catch (err) {

                        console.error(`Error loading image from ${dataSrc}\n${err}`);
                        image.setAttribute("src", "");

                    }

                }

                observer.unobserve(image);

            });

        });

        images.forEach(img => observer.observe(img));

    } else {
        console.log('IntersectionObserver not supported');
    }

}

