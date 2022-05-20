
mergeInto(LibraryManager.library, {
	getBrowserName: function () {
		browserNames = ["Chrome", "Firefox"]
		
		var userAgentNames = navigator.userAgent.match(/[a-zA-Z]+\//g)
		userAgentNames = userAgentNames.map(function (name) {return name.substring(0, name.length - 1)})
		var browserName = null
		
		for (var i = 0; i < userAgentNames.length; i++) {
			if (this.browserNames.indexOf(userAgentNames[i]) >= 0) {
				browserName = userAgentNames[i]
				break
			}
		}
		
		if (browserName == null)
			browserName = ""
		
		var bufferSize = lengthBytesUTF8(browserName) + 1;
		var buffer = _malloc(bufferSize)
		stringToUTF8(browserName, buffer, bufferSize)
		return buffer
	},
	getPlatform: function () {
		var platform = navigator.platform.substring(0, navigator.platform.indexOf(" "))
		
		var bufferSize = lengthBytesUTF8(platform) + 1;
		var buffer = _malloc(bufferSize)
		stringToUTF8(platform, buffer, bufferSize)
		return buffer
	}
})