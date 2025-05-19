// wwwroot/js/reactions.js
document.addEventListener('DOMContentLoaded', function () {
    // Get all reaction buttons
    const reactionButtons = document.querySelectorAll('.reaction-btn');

    // Add click event to each button
    reactionButtons.forEach(button => {
        button.addEventListener('click', function () {
            const postId = this.closest('.reactions').dataset.postId;
            const reactionType = this.dataset.reaction;

            // Send AJAX request to server
            fetch('/Reactions/AddReaction', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: `postId=${postId}&reactionType=${reactionType}`
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // Update reaction counts
                        const container = this.closest('.reactions');
                        container.querySelector('.heart-count').textContent = data.heartCount;
                        container.querySelector('.like-count').textContent = data.likeCount;
                        container.querySelector('.dislike-count').textContent = data.dislikeCount;

                        // Update active state
                        container.querySelectorAll('.reaction-btn').forEach(btn => {
                            btn.classList.remove('active');
                        });

                        // Highlight the selected reaction
                        this.classList.add('active');
                    }
                })
                .catch(error => console.error('Error:', error));
        });
    });

    // Initialize reaction counts and active states
    function initializeReactions() {
        document.querySelectorAll('.reactions').forEach(container => {
            const postId = container.dataset.postId;

            fetch(`/Reactions/GetReactions?postId=${postId}`)
                .then(response => response.json())
                .then(data => {
                    container.querySelector('.heart-count').textContent = data.heartCount;
                    container.querySelector('.like-count').textContent = data.likeCount;
                    container.querySelector('.dislike-count').textContent = data.dislikeCount;

                    // Set active state based on user's previous reaction
                    if (data.userReaction) {
                        const activeButton = container.querySelector(`[data-reaction="${data.userReaction}"]`);
                        if (activeButton) {
                            activeButton.classList.add('active');
                        }
                    }
                })
                .catch(error => console.error('Error:', error));
        });
    }

    // Initialize reactions on page load
    initializeReactions();
});