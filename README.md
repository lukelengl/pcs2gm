# pcs2gm (Planning Center Services to GroupMe)
This command line program allows you to create a new group in GroupMe and automatically add members from one or more teams within a Planning Center Services plan.

## Pre-requisites
### Planning Center authentication
You will need to generate a personal access token for Planning Center's API. To do this, navigate to https://api.planningcenteronline.com/oauth/applications, scroll to the bottom of the page where it says Personal Access Tokens, and click "Create One Now". Type in a description, and then click "Submit". You will now see **Application ID** and **Secret**. 

### GroupMe authentication
You will also need an access token to use GroupMe's API. Navigate to https://dev.groupme.com/applications and click "Create Application". Fill out the form (for callback URL, just use https://example.com), and then click "Create". You will now see **Access Token** at the bottom of the page.

## Running the program

### Setup
After installing the program on your computer, open up Terminal and navigate to the location where you installed it. Then simply type in ./pcs2gm and hit enter.

The first time you run the program, you will be prompted to enter your Planning Center and GroupMe credentials. These will be saved in a file called `config.toml` in the same directory as the program. If you ever need to change these credentials, simply delete the `config.toml` file and run the program again.

### Creating a group
Follow the prompts to create a new group. You will be asked for the service type, which specific service plan, select which teams you want to add, and then enter a name for the group. The program will then create a new group in GroupMe and add the members of the selected teams to the group.

## Disclaimers
This project is not affiliated with Planning Center or GroupMe.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.