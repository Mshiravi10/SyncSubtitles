<h1 align="center">Telegram Subtitle Synchronization Bot</h1>

<p align="center">
  <strong>An innovative Telegram bot designed for seamless subtitle synchronization, leveraging .NET Core and Dependency Injection.</strong>
</p>

<p align="justify">
  This project showcases a Telegram bot built with .NET Core, designed to automate the synchronization of subtitle files. It demonstrates robust software engineering practices such as Dependency Injection for improved modularity and testability. Using the Telegram.Bot library, the bot offers an interactive experience, allowing users to adjust subtitle timings effortlessly. This bot stands out as a tool for video producers, translators, and enthusiasts, simplifying the otherwise complex task of subtitle synchronization.
</p>

<h2>🌟 Key Features</h2>

<ul>
  <li><strong>Interactive Telegram Bot</strong>: Utilizes the <code>Telegram.Bot</code> library for effective user interaction and command handling.</li>
  <li><strong>Dependency Injection</strong>: Implements Dependency Injection using <code>Microsoft.Extensions.DependencyInjection</code> for a clean and modular architecture.</li>
  <li><strong>Time Adjustment</strong>: Offers users the ability to easily specify synchronization times through a user-friendly Telegram interface.</li>
  <li><strong>Subtitle File Processing</strong>: Adjusts subtitle file timestamps accurately, handling various subtitle formats.</li>
</ul>

<h2>🚀 Getting Started</h2>

<h3>Prerequisites</h3>

<ul>
  <li>.NET Core SDK</li>
  <li>Telegram Bot API Token</li>
</ul>

<h3>Installation</h3>

<p>First, clone the repository:</p>

<pre><code>git clone https://github.com/Mshiravi10/SyncSubtitles.git
cd telegram-subtitle-bot</code></pre>

<p>Then, install the necessary dependencies:</p>

<pre><code>dotnet restore</code></pre>

<h3>Configuration</h3>

<p>Create a <code>.env</code> file in the project root and add your Telegram Bot Token:</p>

<pre><code>TELEGRAM_BOT_TOKEN=your_bot_token_here</code></pre>

<h3>Running the Bot</h3>

<p>Start the bot using the following command:</p>

<pre><code>dotnet run</code></pre>

<p>The bot should now be active on Telegram and ready to synchronize subtitles.</p>

<h2>🛠️ Built With</h2>

<ul>
  <li><a href="https://dotnet.microsoft.com/">.NET Core</a>: A cross-platform .NET implementation for websites, servers, and console apps on Windows, Linux, and macOS.</li>
  <li><a href="https://github.com/TelegramBots/Telegram.Bot">Telegram.Bot</a>: A .NET Client for Telegram Bot API.</li>
  <li><a href="https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection">Microsoft.Extensions.DependencyInjection</a>: The default DI container for .NET Core applications.</li>
  <li><a href="https://github.com/bolorundurowb/dotenv.net">dotenv.net</a>: A .NET library to load environment variables from .env files.</li>
</ul>

<h2>🤝 Contributing</h2>

<p>Contributions, issues, and feature requests are welcome! Feel free to check <a href="https://github.com/Mshiravi10/SyncSubtitles/issues">issues page</a>.</p>


<h2>💐 Acknowledgements</h2>

<ul>
  <li>Telegram Bot API</li>
  <li>.NET Core Team</li>
</ul>
