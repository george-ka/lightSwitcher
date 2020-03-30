var apiBaseUrl = "http://localhost:5000/api/v1/switches/";

$('.toggle').minitoggle();
$('.toggle').on("toggle", function(e)
{
	var switchId = $(this).data("switchid")
	var commandUrl = apiBaseUrl + switchId + "?mode=" + (e.isActive ? "on" : "off");
	
	$.post(commandUrl, "", 
		function(data, status, jqXHR)
		{
			console.log("request completed to " + commandUrl)
			console.log(data)
			console.log(status)
		});

	$("#status").html($(this).attr("id"))
		if (e.isActive)
		   ;// $("#status").html("Oh, you turn me on.")
		else
			$(".result").html("No, you turn me off.")
});