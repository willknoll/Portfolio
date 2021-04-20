---
title: Contributor Guide - Get Started - Setup to View Content Changes Locally 
description: This article describes how to view content changes locally, prior to committing.
author: Will Knoll
---

# Set up to view content changes locally

To ensure documentation changes appear as expected, we **require** that you view your updates locally, that is, in a website built on your local machine before submitting them to be published on the production sites.

With a local copy of both the documentation and template files, DocFx can be used to generate the entire site on your own computer. A custom package hosted in Azure Artifacts (run using node.js) assembles all the necessary components.

> [!IMPORTANT]
> Ensure you have completed all of the [prerequisites](omitted.md) before continuing.

## Installation and setup

Complete the following procedures:

* [Connect to feed](#connect-to-feed)
* [Install the latest local build package](#install-the-latest-local-build-package)
* [Clone the project repository](#clone-the-project-repository)
* [Clone the project repository](#clone-the-project-template-repository)
* [Build the local site](#build-the-local-site)

### Connect to feed

The local build tool is hosted as an Azure Artifacts feed. Follow these steps to configure your computer to connect to the feed. Details about the `project-name` project feed can be found [here](omitted.md).

1. Edit your NPM configuration file (`.npmrc`) in Notepad or other text editor. The .npmrc file is located at the root of your users folder (for example, **c:\users\\[user-name]**). If the file does not exist, copy the empty version from your nodejs installation folder `C:\Program Files\nodejs\node_modules\npm` to your users folder. This can be done using PowerShell.

    ```PowerShell
    copy "C:\Program Files\nodejs\node_modules\npm\.npmrc" "c:\users\[user-name]\.npmrc"
    ```

1. Add the following two lines to your **.npmrc** file:

    ```node
    @projectname:registry=https://pkgs.dev.azure.com/org/_packaging/project/npm/registry/
    always-auth=true
    ```

1. Save and close the **.npmrc** file.

1. Store your authentication credentials.

    * **For Windows users**: Install and run the authentication helper by running the following commands in PowerShell. This **must** be done from the same directory as your .npmrc file. Enter or select your &lt;omitted&gt; credentials when prompted.

        ```node
        cd c:\users\[user-name]
        ```

        ```node
        npm install -g vsts-npm-auth --registry https://registry.npmjs.com --always-auth false
        vsts-npm-auth -config .npmrc
        ```

        &lt;image omitted&gt;

### Install the latest local build package

Installing the latest local build package is only required once for setup, but we recommend you perform this step periodically to get the latest updates.

In PowerShell, enter the following command:

```node
npm install -g @projectname/local-build-tool
```

### Clone the project-name repository

Only necessary if you haven't already completed [Set up your local Git repository](omitted.md). The `project-name` repository can be cloned locally using any commandline tool. This example uses PowerShell.

1. Launch PowerShell.

2. Use the **cd** command to change directories to the **Content repository local location**. For example,

    ```code
    cd c:\src
    ```

3. Run the following command:

    ```markdown
    git clone https://github.com/org/project-name
    ```

### Clone the project-template repository

The `project-template` repository can be cloned locally using any commandline tool. This example uses PowerShell.

1. Launch PowerShell.
2. Use the **cd** command to change directories to the **Content repository local location**. For example,

    ```code
    cd c:\src
    ```

3. Run the following command:

    ```markdown
    git clone https://dev.azure.com/org/project/_git/project-template
    ```

### Build the local site

To build the **internal** site:

* From the **PowerShell command prompt**, enter the following command:

    ```node
    build internal
    ```

To build the **external** site:

* From the **PowerShell command prompt**, enter the following command:

    ```node
    build external
    ```

## Additional Information

### Recurring task: view content changes locally

The article [View Content Changes Locally](omitted.md) contains additional information, including parameter descriptions, troubleshooting steps, and other information to help you view your content updates locally before publishing to the production websites.

## Next steps

You are now ready to create a branch and begin editing content. See [Create a branch](omitted.md).
