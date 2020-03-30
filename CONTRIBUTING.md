# How to contribute

Pull requests are always welcome.

## Reporting security issues and bugs
Security issues and bugs should be reported via Jira on the GiG-Core project. 
If for some reason you do not receive feedback within a week, please follow up via slack.


## Contributing code and content

### Identifying the scale

If you would like to contribute to one of our repositories, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix) feel free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with the team and ensure it follows the product roadmap. You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. Note that all code submissions will be rigorously reviewed and tested by the Epic Core team, and only those that meet the bar for both quality and design/roadmap appropriateness will be merged into the source.


### Before Submitting a pull request

Make sure the respository can build and all tests pass. Familiarize yourself with the project workflow and our coding conventions. The coding standards, style, and general pull-request guidelines are published in our [Confluence](https://gaminginnovationgroup.atlassian.net/wiki/spaces/ARCH/pages/371589328/Standards+and+Guidelines) page.


### Tests

Tests need to be provided for every bug/feature that is completed. When encountering scenarios which might hinder the test execution process, or require integrations with 3rd parties that make it almost impossible to test, then there is no need to have integration tests for this. However, as in all the other cases, it is important to at least have unit tests in place and cover most of the functionality. It is important to keep in mind that flagging a scenario as a scenario that will hinder the execution process or impossible to test, is at the discretion of the team as a whole.


### Feedback

Your pull request will now go through extensive checks by the subject matter experts on our team. Please be patient. Update your pull request according to feedback until it is approved by two or more of the Epic Core team members. After that, one of our team members may adjust the branch you merge into based on the expected release schedule.
