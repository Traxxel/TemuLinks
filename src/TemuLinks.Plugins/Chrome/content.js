// Content script for TemuLinks extension
// This script runs on Temu.com pages

(function () {
  "use strict";

  // Check if we're on a Temu.com page (case-insensitive)
  if (!window.location.hostname.toLowerCase().includes("temu.com")) {
    return;
  }

  // Create and inject the TemuLinks button
  function createTemuLinksButton() {
    // Check if button already exists
    if (document.getElementById("temulinks-button")) {
      return;
    }

    const button = document.createElement("div");
    button.id = "temulinks-button";
    button.innerHTML = `
            <div style="
                position: fixed;
                top: 20px;
                right: 20px;
                z-index: 10000;
                background: #007bff;
                color: white;
                padding: 10px 15px;
                border-radius: 5px;
                cursor: pointer;
                font-family: Arial, sans-serif;
                font-size: 14px;
                box-shadow: 0 2px 10px rgba(0,0,0,0.3);
                transition: background-color 0.3s;
            " onmouseover="this.style.backgroundColor='#0056b3'" onmouseout="this.style.backgroundColor='#007bff'">
                📌 Save to TemuLinks
            </div>
        `;

    button.addEventListener("click", function () {
      // Open the extension popup
      chrome.runtime.sendMessage({ action: "openPopup" });
    });

    document.body.appendChild(button);
  }

  // Initialize when DOM is ready
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", createTemuLinksButton);
  } else {
    createTemuLinksButton();
  }

  // Re-create button if page content changes (SPA navigation)
  const observer = new MutationObserver(function (mutations) {
    mutations.forEach(function (mutation) {
      if (mutation.type === "childList" && mutation.addedNodes.length > 0) {
        // Check if our button was removed
        if (!document.getElementById("temulinks-button")) {
          createTemuLinksButton();
        }
      }
    });
  });

  observer.observe(document.body, {
    childList: true,
    subtree: true,
  });
})();
