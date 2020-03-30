var apiBaseUrl = document.location.href + "api/v1/switches/";

$(document).ready(function()
{
	var initToggling = function()
	{
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

			$("#status").html($(this).attr("id"));
		});
	};

	var getStatus = function()
	{
		var getStateUrl = apiBaseUrl;
		$.get(getStateUrl, "", 
			function(data, status, jqXHR)
			{
				if (status != "success")
				{
					console.log("request to " + commandUrl + " completed unsuccessfully.")
					console.log(data)
					console.log(status)	
				}
				
				for (var i = 0; i < data.length; i++)
				{
					$("#switch" + data[i]["switchId"]).minitoggle({ on : data[i]["state"] });
				}
				
				initToggling();
			});
	};

	getStatus();

});
