plugins { java; application }
repositories { mavenCentral() }
java {
    toolchain {
        languageVersion.set(JavaLanguageVersion.of(27))
    }
}
tasks.withType<JavaCompile>().configureEach {
    options.release.set(27)
    options.encoding = "UTF-8"
}
dependencies {
    testImplementation(platform("org.junit:junit-bom:6.1.3"))
    testImplementation("org.junit.jupiter:junit-jupiter")
    testRuntimeOnly("org.junit.platform:junit-platform-launcher")
}
tasks.test { useJUnitPlatform() }
application { mainClass.set("ua.knu.loan.Loan") }
