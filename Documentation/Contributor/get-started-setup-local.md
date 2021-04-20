---
title: Contributor Guide - Get Started - Set up your local repository
description: This article provides guidance to create a local repository (including the forking and cloning process).
author: Will Knoll
---

# Set up your local Git repository

This article describes the steps to set up a Git repository (*repo*) on your computer so you can contribute to &lt;project&gt; documentation. Contributors use a locally-cloned repo to add new articles, perform major edits on existing articles, and change artwork. **This is required so you can get started contributing**.

## Create source directory on your local machine

Create a directory that will contain your repository. We recommend that you name this directory **C:\src**.

1. Open PowerShell
1. `mkdir c:\src`
1. `cd c:\src`

## Set up your local repository

To set up your new local repository:

1. Navigate to &lt;project&gt; on GitHub.com.
1. Select **Fork**.
1. In the dialog box, select your GitHub user name.
1. Select **Clone or download**.
1. In the Clone with HTTPs section, click ![Copy to clipboard](copy-to-clipboard.png) to copy the clone URL for the repository.<br><br>&lt;image omitted&gt;<br><br>
1. In PowerShell type ```git clone```, and paste the URL you copied. It will look like this, with your GitHub username instead of ```YOUR-USERNAME```:

    ```git
    git clone https://github.com/YOUR-USERNAME/project.git
    ```

1. Press **Enter**. Your local clone will be created.

## Next steps

See [Set up to view content changes locally](get-started-setup-view-content-changes-locally.md).
