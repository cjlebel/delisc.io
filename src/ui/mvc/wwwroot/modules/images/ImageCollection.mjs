export default function ImageCollection(containerId) {
    if (!containerId)
        return;

    const container = document.getElementById(containerId);
    const images = container.querySelectorAll('img');

    images.forEach(image => {
        if (!image.dataset.src)
            return;


    });
}