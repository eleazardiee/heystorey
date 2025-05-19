// Initialize when the document is ready
document.addEventListener('DOMContentLoaded', function () {
    // Create post button click handler
    const createPostBtn = document.getElementById('createPostBtn');
    const createPostModal = new bootstrap.Modal(document.getElementById('createPostModal'));

    if (createPostBtn) {
        createPostBtn.addEventListener('click', function () {
            createPostModal.show();
        });
    }

    // Image preview functionality
    const imageInput = document.querySelector('input[type="file"][multiple]');
    const previewContainer = document.getElementById('imagePreviewContainer');

    if (imageInput && previewContainer) {
        imageInput.addEventListener('change', function () {
            // Clear existing previews
            previewContainer.innerHTML = '';

            // Create previews for each selected file
            for (const file of this.files) {
                if (file.type.startsWith('image/')) {
                    const reader = new FileReader();

                    reader.onload = function (e) {
                        const previewDiv = document.createElement('div');
                        previewDiv.className = 'col-4 col-md-3 mb-2';

                        const img = document.createElement('img');
                        img.src = e.target.result;
                        img.className = 'img-thumbnail';
                        img.style.height = '120px';
                        img.style.width = '100%';
                        img.style.objectFit = 'cover';

                        previewDiv.appendChild(img);
                        previewContainer.appendChild(previewDiv);
                    };

                    reader.readAsDataURL(file);
                }
            }
        });
    }
});