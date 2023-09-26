<h1>PokemonReview ASP.NET Web API Project </h1><br/>
<strong>This ASP.NET Web API project, called PokemonReview, is designed to manage and review Pokemon data. 
The project utilizes a SQL Server database to store information related to Pokemon, their owners, categories, countries, reviews, and reviewers.
</strong>

<h3>Project Structure</h3>
<span>The project is structured as follows:</span>
<ul>
  <li><b>Pokemon Table:</b>  Contains information about each Pokemon, including their details and associated data.</li>
  <li><b>Owner Table: </b>  Stores information about Pokemon owners, linked to their respective countries via a one-to-many relationship.</li>
  <li><b>Country Table:</b>  Holds data about countries, associated with Pokemon owners through a one-to-many relationship.</li>
  <li><b>Review Table:</b>  Contains reviews for each Pokemon, tied to their respective reviewers, forming a many-to-many relationship.</li>
  <li><b>Reviewer Table:</b>  Stores data about the reviewers who write reviews for Pokemon.</li>
  <li><b>PokemonCategory Table:</b>  Manages the relationship between Pokemon and their categories, forming a many-to-many relationship.
</li>
</ul>

<h2>Usage and Contributions</h2><br/>
This project is open for contributions and improvements. Feel free to fork the repository, make changes, and submit a pull request to enhance the functionality, efficiency, or usability of the project. Any contributions are welcome and appreciated.
