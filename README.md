# ReflectedXSSOnQueryParam


Methodology for Black Box Testing of Reflected XSS on Query Parameters
```C#
//Given the input URI, search for all input query parameters (The C# Solution assumes that this step is completed for simplicity) 
queryParamList = getQueryParam(URI);

foreach(queryParam in queryParamList)
{
	//Initial Probe
	queryParam.value = RandomString();
	response = GetResponseFromRequest(URI + queryParam);

	if(!response.contains(RandomString))
	{
		//This query parameter can't be attacked
		continue;
	}

	//start xss attacks
	foreach(attackString in AttackStringList)
	{
		queryParam.value = attackString;
		response = GetResponseFromRequest(URI + queryParam);

		if(probeResponse(response)) //probe response checks to see if the attack string was injected at a place where it might be executable
		{
			Console.WriteLine("Vulnerable");
			break;
		}
		else
		{
			// This attack string was fuzzed by the server, try the next one
			continue;
		}

	}

}


/*
	Improvements:
	- For every URI , grab all the redirects and test them for XSS Vulns as well.
	- To test for DOM based XSS, run the response HTML (from the server) through some script 
	  executer to check if the clients browser injects the parameter values in the HTML 
	  rather than the server. 
*/
```
