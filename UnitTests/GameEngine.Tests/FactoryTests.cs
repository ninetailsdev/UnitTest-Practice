using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameEngine.Tests
{
    public class FactoryTests
    {

        [Fact]
        public void CreateNormalEnemyByDefault()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie");

            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact]
        public void CreateNormalEnemyByDefault_NotTypeExample()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie");

            Assert.IsNotType<DateTime>(enemy);
        }

        [Fact]
        public void CreateBossEnemyByDefault()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie Baby", true);

            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        public void CreateBossEnemyByDefault_CastReturnedType()
        {
            var sut = new EnemyFactory();
            var enemy = sut.Create("Zombie Baby", true);

            var boss = Assert.IsType<BossEnemy>(enemy);

            Assert.Equal("Zombie King", boss.Name);
        }

    }
}
