# Data Privacy utilities for Sitecore 9.x

Data privacy is a hot topic and the GDPR enforces conscious use of your visitors data. Sitecore hands you some tools like the Right to be Forgotten functionality, but there's no automatic disposal of data out-of-the-box to match your privacy policy, and the right to be forgotten method doesn't clean up all the data in Sitecore, like saved Forms submissions. In this repository I demonstrate some examples of how to elegantly implement automatic data disposal and also cover data that isn't included in this process normally.

## Sliding Expiration
A new (custom) activity type for Marketing Automation plans to execute the right to be forgotten on the contact in the plan, named 'AnonymizeContact'. Additionaly, this example implementation of sliding expiration contians an automation plan template to anonymize contacts that did not interact with any channel for over a certain amount of time, by default 2 years. This creates a sliding expiration, executing the RightToBeForgotten method on all contacts you no longer are allowed to keep in your xDB records. By default, the Sitecore platform only lets you configure cookie lifetime, which disconnects the user and his behavior from the xDB profile, but doesn't remove any PII data you may have collected. This plan hands you the most elegant solution to do so, while also enabling you to incorporate anonymizing contacts in other automation plan flows (like at the end of a campaign, when you no longer need the data).

_See the 'Sitecore modules' folder for an installable Sitecore module (and don't forget to deploy the required files from the separate xConnect archive to your xConnect role) and the Habitat demo for the actual code examples. You can use the same Helix Feature in your own project and further customize it to your liking._

## Disposal of saved Form data for Marketing Automation plans
A new (custom) activity type for Marketing Automation plans to erase form submission data stored in the ExperienceForms database, named 'EraseFormSubmissions'. This activity type requires a custom submit action for Sitecore Forms, which is also included in the module, that stores the xDB.Tracker ID as a hidden field in each form submission, to be able to establish a connection between a Contact and a FormEntry. Be aware that if used together with the AnonymizeContact marketing action, you should perform this action first, as the other one would erase the xDB.Tracker ID by executing the right to be forgotten, effectively removing the connection between the contact and all its form entries.

## Automatic disposal of saved Form data upon executing the Right to be Forgotten
Thanks to the contribution of @Antonytm this repository now contains an xConnect service plugin that automatically erases form submission data of a contact upon executing the right to be forgotten. It does not matter how this method is initiated, either from the dashboard of an Experience Profile, via the marketion action 'AnonymizeContact' as included in this repository, or via custom code. This addition allows you to only use the 'AnonymizeContact' action in a marketing automation plan, without the need of adding the 'EraseFormSubmissions' action as well. But more importantly, it ensures that your form data is cleaned up regardless of the method being used. This functionality is included in the 2.0 version of the installable EraseFormSubmissions module, as well as in the Habitat demo project.

Having this service plugin doesn't render the 'EraseFormSubmissions' action useless, as you can still use that in other scenarios like in a marketing automation plan for specific campaigns, or when your form entries have a shorter retention period than your general privacy policy states.

_See the 'Sitecore modules' folder for an installable Sitecore module (and don't forget to deploy the required files from the separate xConnect archive to your xConnect role; also note that you need to add the connection string for the forms database to your xConnect deployment as well) and the Habitat demo for the actual code examples. You can use the same Helix Feature in your own project and further customize it to your liking._

## License info
**MIT License**\
Copyright (c) 2020 We are you, the Netherlands

Permission is hereby granted, free of charge, to any person obtaining a copy\
of this software and associated documentation files (the "Software"), to deal\
in the Software without restriction, including without limitation the rights\
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell\
copies of the Software, and to permit persons to whom the Software is\
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all\
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR\
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,\
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE\
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER\
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,\
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE\
SOFTWARE.

_NB: For Habitat Corporate project, see corresponding LICENSE file in sub folder of Habitat demo_
