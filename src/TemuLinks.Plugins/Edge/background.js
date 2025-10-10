// Background service worker for TemuLinks extension

chrome.runtime.onInstalled.addListener(function (details) {
  if (details.reason === "install") {
    // Set default configuration
    chrome.storage.sync.set({
      apiEndpoint: "http://localhost:5002",
      apiKey: "GpRWMrrnJYGVZERXLLBCNje1kfLjrBBO",
    });
  }
});

// Handle messages from content scripts
chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
  if (request.action === "saveLink") {
    // Handle link saving logic if needed
    sendResponse({ success: true });
  }
});


