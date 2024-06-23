/**
 * Both Lazy Loads are here, so Vite won't create a separate StripDataAttributes file
 */
import { StripDataAttributes } from '../dom';

/**
 * Looks up all images that have the 'lazy' class and lazy loads them.
 */

function LazyLoadImage() {
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

/**
* Looks up all videos that are in divs with the 'videoLazyLoad' class and lazy loads them.
*/
function LazyLoadVideo() {

    const lazyVideos = [...document.querySelectorAll('div.lazy-video')];

    if ('IntersectionObserver' in window) {
        let lazyImageObserver = new IntersectionObserver(function (entries, observer) {
            entries.forEach(function (entry) {

                if (entry.isIntersecting) {

                    const videoElement = entry.target as HTMLElement;

                    startLazyLoadVideoMode(videoElement);

                    observer.unobserve(entry.target);
                }

            });

        });

        lazyVideos.forEach(function (lazyVideo) {
            lazyImageObserver.observe(lazyVideo);
        });
    } else {

        // In case it's an older browser that doesn't support 'IntersectionObserver'

    }


    /**
     * Not tested
     * @param {Element} lazyVideo
     */
    function startLazyLoadVideoMode(lazyVideo: HTMLElement) {
        var videoItem = document.createElement('div');

        const videoId = lazyVideo.dataset.id ?? ''

        if (videoId === '')
            return;

        videoItem.setAttribute('data-id', videoId);

        const vidCssClass = lazyVideo.dataset.vidclass ?? '';
        const imgCssClass = lazyVideo.dataset.imgclass ?? '';

        let imgWidth = 0;
        let imgHeight = 0;

        if (lazyVideo.dataset.imgwidth !== undefined) {
            let w = parseInt(lazyVideo.dataset.imgwidth);

            if (!isNaN(w) && w > 0) {
                imgWidth = w;
            }
        }

        if (lazyVideo.dataset.imgheight !== undefined) {
            let h = parseInt(lazyVideo.dataset.imgheight);

            if (!isNaN(h) && h > 0) {
                imgHeight = h;
            }
        }

        // data-img allows us to use a custom thumbnail
        const imgPath = lazyVideo.dataset.img ?? '';

        // Original js checked for cssClass. Not sure why.
        //if (cssClass !== '') {

        if (imgPath !== '') {
            videoItem.innerHTML = setVideoThumb(imgPath, imgCssClass, imgWidth, imgHeight);
        }
        else
            videoItem.innerHTML = getVideoThumb(videoId, imgCssClass);

        //}

        const videoWidth = lazyVideo.dataset.width ?? '';
        if (videoWidth !== '')
            videoItem.setAttribute('data-width', videoWidth);

        const videoHeight = lazyVideo.dataset.height ?? '';
        if (videoHeight !== '')
            videoItem.setAttribute('data-height', videoHeight);

        const videoLoop = lazyVideo.dataset.loop ?? '';
        if (videoLoop !== '')
            videoItem.setAttribute('data-loop', videoLoop);

        // Creates iframe that gets activated on click
        videoItem.onclick = () => {
            setVideoIframe(videoItem, vidCssClass);
        };

        lazyVideo.appendChild(videoItem);

        StripDataAttributes(lazyVideo);
    }



    /**
     * Gets the video's thumbnail from Youtube itself
     * @param {string} id
     * @param {string} cssClass
     * @returns The thumbnail image as a string. Try to return HtmlImageElement (or HtmlElement)
     */
    function getVideoThumb(id: string, cssClass: string): string {

        // Various versions of the thumbnail
        //var thumb = `<img src="https://i.ytimg.com/vi/${id}/default.jpg" class="${cssClass}"><span></span>`;
        var thumb = `<img src="https://i.ytimg.com/vi/${id}/mqdefault.jpg" class="${cssClass}"><span></span>`;
        //var thumb = `<img src="https://i.ytimg.com/vi/${id}/hqdefault.jpg" class="${cssClass}"><span></span>`;
        //var thumb = `<img src="https://i.ytimg.com/vi/${id}/1.jpg" class="${cssClass}"><span></span>`;
        //var thumb = `<img src="https://i.ytimg.com/vi/${id}/2.jpg" class="${cssClass}"><span></span>`;
        //var thumb = `<img src="https://i.ytimg.com/vi/${id}/3.jpg" class="${cssClass}"><span></span>`;

        return thumb;

        // TS version
        //var thumbNail = new HTMLImageElement();
        //thumbNail.src = 'https://i.ytimg.com/vi/{id}/hqdefault.jpg';
        //thumbNail.classList.add(cssClass);

        //return thumbNail;

    }

    /**
     * Sets the video's thumbnail using a custom image
     * @param url The url or path to the thumbnail file
     * @param cssClass css class(es) to be added to the thumbnail
     * @param originalWidth the thumbnails intrinsic (original) width
     * @param originalHeight the thumbnails intrinsic (original) height
     * @returns
     */
    function setVideoThumb(url: string, cssClass: string, originalWidth: number | null, originalHeight: number | null): string {

        var thumb = `<img src="${url}" class="${cssClass}"`;

        if (originalWidth != null) {
            thumb += ` width="${originalWidth}"`;
        }

        if (originalHeight != null) {
            thumb += ` height="${originalHeight}"`;
        }

        thumb += '/>';

        return thumb;

        // TS version
        //var thumbNail = new HTMLImageElement();
        //thumbNail.src = url;
        //thumbNail.classList.add(cssClass);

        //return thumbNail;
    }


    function setVideoIframe(video: HTMLDivElement, vidCssClass: string | null) {
        var iframe = document.createElement('iframe');
        var playInLoop = video.dataset.loop === 'true';

        var src = `https://www.youtube.com/embed/${video.dataset.id}`;
        src += `?autoplay=1`;
        src += playInLoop ? `&playlist=${video.dataset.id}&loop=1` : '';
        // mute needs to be enabled for autoplay to work when the user clicks the cover image
        src += `&mute=1`;

        iframe.setAttribute('src', src);
        iframe.setAttribute('allowfullscreen', '1');
        iframe.setAttribute('frameborder', '0');
        iframe.setAttribute('width', video.dataset.width ?? '');
        iframe.setAttribute('height', video.dataset.height ?? '');
        iframe.setAttribute('title', video.dataset.title ?? '');

        if (vidCssClass) {
            iframe.classList.add(vidCssClass);
        }

        if (video.parentNode)
            video.parentNode.replaceChild(iframe, video);


        /* Example:
            <iframe class="rounded-4"
                    width="100%"
                    height="306"
                    frameborder="0"
                    controls="0"
                    rel="0"
                    src="https://www.youtube.com/embed/9b9KgmtcJXw"
                    title="StickerYou | Make What Matters Stick!"
                    frameborder="0"
                    allow="
                 accelerometer;
                 autoplay;
                 clipboard-write;
                 encrypted-media;
                 gyroscope;
                 picture-in-picture;
                 web-share"
                    referrerpolicy="strict-origin-when-cross-origin"
                    allowfullscreen>
            </iframe>
        */
    }
}

export { LazyLoadImage, LazyLoadVideo }