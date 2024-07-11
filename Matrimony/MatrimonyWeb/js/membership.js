async function viewMembershipNew(currentProfile) {
    const existingModal = document.getElementById('membershipModal');
    if (existingModal) {
        existingModal.remove();
    }
    if (!currentProfile) {
        showAlert("No profile selected", 'danger');
        return;
    }

    let modalHtml = `
    <div class="modal fade" id="membershipModal" tabindex="-1" aria-labelledby="membershipModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="membershipModalLabel">Membership Details</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body">
                <div id="currentMembership" class="card mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Current Membership</h5>
                        <p class="card-text" id="membershipType"><strong>Type:</strong> <span></span></p>
                        <p class="card-text" id="membershipEndsAt"><strong>Ends At:</strong> <span></span></p>
                        <p class="card-text" id="membershipViewsCount"><strong>Views Count:</strong> <span></span></p>
                        <p class="card-text" id="membershipChatCount"><strong>Chat Count:</strong> <span></span></p>
                        <p class="card-text" id="membershipRequestCount"><strong>Request Count:</strong> <span></span></p>
                        <p class="card-text" id="membershipViewersViewCount"><strong>Viewers View Count:</strong> <span></span></p>
                        <p class="card-text"> <span> Please Contact admins if you want to update your membership.</span></p>
                    </div>
                </div>
                <h5>Available Plans</h5>
                <div class="row">
                    <div class="col-md-4">
                        <div class="card mb-4">
                            <div class="card-body">
                                <h5 class="card-title">Free Member $0</h5>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item">5 matches per month</li>
                                    <li class="list-group-item">No private chat</li>
                                    <li class="list-group-item">No profile views</li>
                                    <li class="list-group-item">Only profile Preview</li>
                                </ul>
                                <button class="btn btn-primary mt-3 w-100 d-none" id="freeButton">Current Plan</button>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card mb-4">
                            <div class="card-body">
                                <h5 class="card-title">Basic Member $5</h5>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item">15 matches per month</li>
                                    <li class="list-group-item">No private chats</li>
                                    <li class="list-group-item">Up to 5 views per month</li>
                                    <li class="list-group-item">50 detailed profile lookups</li>
                                </ul>
                                <button class="btn btn-primary mt-3 w-100" id="basicButton" onclick="initiatePayment(5, 'basic')">Upgrade to Basic</button>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card mb-4">
                            <div class="card-body">
                                <h5 class="card-title">Premium Member $15</h5>
                                <ul class="list-group list-group-flush">
                                    <li class="list-group-item">Unlimited matches per month</li>
                                    <li class="list-group-item">Up to 25 private chats</li>
                                    <li class="list-group-item">Unlimited profile views</li>
                                    <li class="list-group-item">Unlimited detailed profile lookups</li>
                                </ul>
                                <button class="btn btn-primary mt-3 w-100" id="premiumButton" onclick="initiatePayment(15, 'premium')">Upgrade to Premium</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary" onclick="contactAdmin()">Contact Admin</button>
            </div>
        </div>
    </div>
</div>
    `
    const modalContainer = document.createElement('div');
    modalContainer.innerHTML = modalHtml;
    document.body.appendChild(modalContainer);

    try {
        const membership = await makeAuthRequest(`Membership/profile/${currentProfile}`);
        if (membership) {
            document.getElementById('membershipType').querySelector('span').textContent = membership.type;
            document.getElementById('membershipEndsAt').querySelector('span').textContent = new Date(membership.endsAt).toLocaleDateString();
            document.getElementById('membershipViewsCount').querySelector('span').textContent = membership.viewsCount;
            document.getElementById('membershipChatCount').querySelector('span').textContent = membership.chatCount;
            document.getElementById('membershipRequestCount').querySelector('span').textContent = membership.requestCount;
            document.getElementById('membershipViewersViewCount').querySelector('span').textContent = membership.viewersViewCount;

            // Hide all buttons initially
            const buttons = ['freeButton', 'basicButton', 'premiumButton'];
            buttons.forEach(btn => {
                // document.getElementById(btn).classList.add('d-none');
            });

            // Show and style the current plan button
            let currentButton;
            switch (membership.type.toLowerCase()) {
                case 'free':
                    currentButton = document.getElementById('freeButton');
                    break;
                case 'basic':
                case 'basicuser':
                    currentButton = document.getElementById('basicButton');
                    break;
                case 'premium':
                case 'premiumuser':
                    currentButton = document.getElementById('premiumButton');
                    break;
            }

            if (currentButton) {
                currentButton.classList.remove('d-none');
                currentButton.classList.replace('btn-primary', 'btn-secondary');
                currentButton.textContent = 'Current Plan';
                currentButton.disabled = true;
            }

            let membershipModal = new bootstrap.Modal(document.getElementById('membershipModal'));
            membershipModal.show();
        } else {
            showAlert("Failed to fetch membership details", 'danger');
        }
    } catch (error) {
        console.error('Error fetching membership:', error);
        showAlert("An error occurred while fetching membership details", 'danger');
    }
}

function initiatePayment(amount, membershipType) {
    const options = {
        key: "YOUR_RAZORPAY_KEY", // Replace with your Razorpay key
        amount: amount * 100, // Amount in paise
        currency: "USD",
        name: "Your Site Name",
        description: `Upgrade to ${membershipType} Membership`,
        handler: function (response) {
            // Handle the success response from Razorpay
            console.log(response);
            upgradeMembership(membershipType);
        },
        prefill: {
            name: "Customer Name", // Replace with the customer name
            email: "customer@example.com", // Replace with the customer email
        },
        theme: {
            color: "#3399cc"
        }
    };
    const rzp1 = new Razorpay(options);
    rzp1.open();
}

// async function upgradeMembership(membershipType) {
//     const currentProfile = localStorage.getItem('currentProfile');
//     if (!currentProfile) {
//         showAlert("No profile selected", 'danger');
//         return;
//     }
//
//     try {
//         const dto = {
//             type: membershipType,
//             profileId: currentProfile
//         };
//         const response = await makeAuthRequest('Membership/update', 'PUT', dto);
//         if (response) {
//             showAlert("Membership upgraded successfully", 'success');
//             viewMembershipNew(currentProfile); // Refresh the membership details
//         } else {
//             showAlert("Failed to upgrade membership", 'danger');
//         }
//     } catch (error) {
//         console.error('Error upgrading membership:', error);
//         showAlert("An error occurred while upgrading membership", 'danger');
//     }
// }
async function upgradeMembership(membershipType) {
    const currentProfile = localStorage.getItem('currentProfile');
    if (!currentProfile) {
        showAlert("No profile selected", 'danger');
        return;
    }

    try {
        const paymentDetails = {
            type: membershipType,
            profileId: currentProfile,
            paymentId: paymentResponse.razorpay_payment_id, // Add Razorpay payment ID here
            paymentStatus: 'completed',
            paymentDate: new Date().toISOString(),
            amountPaid: amount // Add the amount paid
        };

        const response = await makeAuthRequest('Membership/update', 'PUT', paymentDetails);
        if (response) {
            showAlert("Membership upgraded successfully", 'success');
            await viewMembershipNew(currentProfile); // Refresh the membership details
        } else {
            showAlert("Failed to upgrade membership", 'danger');
        }
    } catch (error) {
        console.error('Error upgrading membership:', error);
        showAlert("An error occurred while upgrading membership", 'danger');
    }
}
