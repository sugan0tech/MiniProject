function removeMatch(category, matchId) {
    // Function to handle removing a match based on category
    console.log(`Remove match ${matchId} from ${category} category`);
    const matchCard = document.querySelector(`#${category}Matches [data-match-id="${matchId}"]`);
    if (matchCard) {
        matchCard.remove();
    }
}

function acceptMatch(matchId) {
    // Function to handle accepting a match request
    console.log(`Accept match request with ID: ${matchId}`);
    const matchCard = document.querySelector(`#receivedMatches [data-match-id="${matchId}"]`);
    if (matchCard) {
        matchCard.remove();
        // Add the match to the accepted matches list
        document.getElementById('acceptedMatches').innerHTML += `
            <div class="card mb-3" data-match-id="${matchId}">
                <div class="card-body">
                    <h5 class="card-title">${matchCard.querySelector('.card-title').textContent}</h5>
                    <p class="card-text">${matchCard.querySelector('.card-text').textContent}</p>
                    <button class="btn btn-danger" onclick="removeMatch('accepted', ${matchId})">Remove Match</button>
                </div>
            </div>
        `;
    }
}
