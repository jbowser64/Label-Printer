# Label-Printer
<H1>TA Label Printer</H1>
<h2>This is still a work in progress!</h2>
Creating a locally hosted site in Visual Studio Community using .Net MVC and Entity Framework. This site will allow for team members to print labels for parts in our warehouse and for production. The program uses a Labelary API endpoint to generate a PNG image as a label. 

<h1>VS Downloads Needed!(Visual Studio Installer)</h1>
  <list>
  <li>
   ASP.NET and web development
  </li>
  </list>
<h1>Nuget Packs Needed!(nuget package installer inside Visual Studio)</h1>

<list>
  <li>
    EntityFramework
  </li>
  <li>
    Microsoft.EntityFrameworkCore
</li>
  <li>
    Microsoft.EntityFrameworkCore.Design
</li>
  <li>
     Microsoft.EntityFrameworkCore.SqlServer
</li>
  <li>
     Microsoft.EntityFrameworkCore.Tools
</li>
</list>
<h1>To seed Database and connect to program:</h1>
<ol>
  <li>
    Use SQL server of your choosing
  </li>
  <li>
    Import "PartsAndLocationsSQLite.CSV" file from lblprint>Data folder.
  </li>
  <li>
   Check the " Use first row as column names" box and import
  </li>
  <li>
    Connect database to Visual Studio project using the server explorer
  </li>
  <li>
    Navigate to lblprint>Models>PartsAndLocationsContext.cs and change line 19 to => optionsBuilder.UseSqlServer("Data Source= {YOUR SQL CONNNECTION HERE}) 
  </li>
</ol>
